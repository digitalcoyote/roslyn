// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Debugger.Contracts.EditAndContinue;

namespace Microsoft.CodeAnalysis.ExternalAccess.DotNetWatch
{
    [Export]
    [Shared]
    internal sealed class DotNetEditAndContinueWorkspaceService
    {
        private readonly SolutionActiveStatementSpanProvider _nullActiveStatementSpanProvider = (_, __) => new(ImmutableArray<TextSpan>.Empty);
        private readonly Lazy<IEditAndContinueWorkspaceService> _workspaceService;

        [ImportingConstructor]
        [Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
        public DebuggerFindReferencesService(
            IThreadingContext threadingContext,
            Lazy<IEditAndContinueWorkspaceService> workspaceService)
        {
            _workspaceService = workspaceService;
        }

        public Task OnSourceFileUpdatedAsync(Document document, CancellationToken cancellationToken)
            => _workspaceService.OnSourceFileUpdatedAsync(document, cancellationToken);

        public void CommitSolutionUpdate() => _workspaceService.CommitSolutionUpdate();

        public void DiscardSolutionUpdate() => _workspaceService.DiscardSolutionUpdate();

        public void EndDebuggingSession(Solution solution) => _workspaceService.EndDebuggingSession(solution);

        public void StartDebuggingSession(Solution solution) => _workspaceService.StartDebuggingSession(solution);

        public void StartEditSession() => _workspaceService.StartEditSession(tubManagedEditAndContinueDebuggerService.Instance, out _);

        public void EndEditSession() => _workspaceService.EndEditSession(out _);

        public async ValueTask<DotNetWatchManagedModuleUpdates> EmitSolutionUpdateAsync(Solution solution)
        {
            var (updates, _) = await EmitSolutionUpdateAsync(solution, _nullActiveStatementSpanProvider, cancellationToken).ConfigureAwait(false);

            var forwardingUpdates = new DotNetWatchManagedModuleUpdates(
                (DotNetWatchManagedModuleUpdateStatus)updates.Status,
                ImmutableArray.CreateRange(updates.Updates.Select(u => new DotNetWatchManagedModuleUpdate(u.Module, u.ILDelta, u.MetadataDelta, u.PdbDelta, u.UpdatedMethods))));

            return (forwardingUpdates);
        }
    }
}
