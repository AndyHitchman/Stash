#region License

// Copyright 2009 Andrew Hitchman
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// 	http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.

#endregion

namespace Stash.Engine.PersistenceEvents
{
    using System;

    public class Update<TGraph> : PersistenceEvent<TGraph>
    {
        private readonly Track<TGraph> trackEvent;

        public Update(Track<TGraph> trackEvent)
        {
            this.trackEvent = trackEvent;
        }

        public object UntypedGraph
        {
            get { return trackEvent.UntypedGraph; }
        }

        public TGraph Graph
        {
            get { return trackEvent.Graph; }
        }

        public IInternalSession Session
        {
            get { return trackEvent.Session; }
        }

        public void Complete()
        {
            throw new NotImplementedException();
        }

        public void EnrollInSession()
        {
            Session.Enroll(this);
        }

        public PreviouslyEnrolledEvent SayWhatToDoWithPreviouslyEnrolledEvent(IPersistenceEvent @event)
        {
            return PreviouslyEnrolledEvent.ShouldBeRetained;
        }
    }
}