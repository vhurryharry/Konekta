using System;
using System.ServiceModel;

namespace WCA.FirstTitle.Client
{
    internal class WCFHelpers
    {
        /// <summary>
        /// Closes com objects in the order passed in if not already closed.
        /// If Close fails for Timeout or Communication exception then Aborts.
        ///
        /// From https://github.com/dotnet/wcf/blob/8589daec6cd3d7d4b1e90238cae951a91e937c03/src/System.Private.ServiceModel/tests/Common/Scenarios/ScenarioTestHelpers.cs
        /// Under licence:
        ///     Licensed to the .NET Foundation under one or more agreements.
        ///     The .NET Foundation licenses this file to you under the MIT license.
        ///     See the LICENSE file in the project root for more information.
        /// </summary>
        /// <param name="objects">Any communication objects that need to be cleaned up.
        /// In the order in which they need to be cleaned up.</param>
        public static void CloseCommunicationObjects(params ICommunicationObject[] objects)
        {
            foreach (ICommunicationObject comObj in objects)
            {
                try
                {
                    if (comObj == null)
                    {
                        continue;
                    }
                    // Only want to call Close if it is in the Opened state
                    if (comObj.State == CommunicationState.Opened)
                    {
                        comObj.Close();
                    }
                    // Anything not closed by this point should be aborted
                    if (comObj.State != CommunicationState.Closed)
                    {
                        comObj.Abort();
                    }
                }
                catch (TimeoutException)
                {
                    comObj.Abort();
                }
                catch (CommunicationException)
                {
                    comObj.Abort();
                }
            }
        }
    }
}
