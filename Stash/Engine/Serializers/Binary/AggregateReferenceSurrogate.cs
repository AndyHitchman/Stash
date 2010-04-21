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

    public class AggregateReferenceSurrogate : ISerializationSurrogate
    {
        public const string ReferenceInfoKey = "___StashInternalId";

        /// <summary>
        /// Populates the provided <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the object.
        /// </summary>
        /// <param name="obj">The object to serialize. 
        ///                 </param><param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data. 
        ///                 </param><param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization. 
        ///                 </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. 
        ///                 </exception>
        public virtual void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var session = context.Context as ISerializationSession;
            if(session == null)
                throw new ArgumentException("context does not contain an instance of IInternalSession");

            var internalId = session.InternalIdOfTrackedGraph(obj);

            if(internalId == null)
                throw new InvalidOperationException(
                    string.Format("Graph of type {0} ({1}) is not tracked and therefore cannot be serialised as a reference", obj.GetType(), obj));

            info.SetType(typeof(AggregateProxy));
            info.AddValue(ReferenceInfoKey, internalId);
        }

        /// <summary>
        /// Populates the object using the information in the <see cref="T:System.Runtime.Serialization.SerializationInfo"/>.
        /// </summary>
        /// <returns>
        /// The populated deserialized object.
        /// </returns>
        /// <param name="obj">The object to populate. 
        ///                 </param><param name="info">The information to populate the object. 
        ///                 </param><param name="context">The source from which the object is deserialized. 
        ///                 </param><param name="selector">The surrogate selector where the search for a compatible surrogate begins. 
        ///                 </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. 
        ///                 </exception>
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            return null;
        }
    }
}