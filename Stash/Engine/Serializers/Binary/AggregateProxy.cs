#region License
// Copyright 2009, 2010 Andrew Hitchman
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

namespace Stash.Engine.Serializers.Binary
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class AggregateProxy : ISerializable, IObjectReference
    {
        private readonly InternalId internalId;

        public AggregateProxy(SerializationInfo info, StreamingContext context)
        {
            //Return a reference to a tracked object if available.
            var rawInternalId = info.GetValue(AggregateReferenceSurrogate.ReferenceInfoKey, typeof(InternalId));

            if (rawInternalId == null)
                throw new InvalidOperationException("Expected SerializationInfo to contain an internal id for an aggregate");

            internalId = (InternalId)rawInternalId;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public object GetRealObject(StreamingContext context)
        {
            var session = context.Context as ISerializationSession;
            if(session == null)
                throw new ArgumentException("context does not contain an instance of ISerializationSession");

            return session.TrackedGraphForInternalId(internalId);
        }
    }
}