// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Immutable;

namespace Microsoft.CodeAnalysis.EditAndContinue
{
    internal readonly struct EditAndContinueManagedModuleUpdates
    {
        public readonly EditAndContinueManagedModuleUpdateStatus Status;

        public readonly ImmutableArray<EditAndContinueManagedModuleUpdate> Updates;

        public EditAndContinueManagedModuleUpdates(EditAndContinueManagedModuleUpdateStatus status, ImmutableArray<EditAndContinueManagedModuleUpdate> updates)
        {
            Status = status;
            Updates = updates;
        }
    }
}
