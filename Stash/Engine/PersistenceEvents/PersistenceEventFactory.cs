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
    using System.IO;

    public interface PersistenceEventFactory
    {
        Destroy<TGraph> MakeDestroy<TGraph>(Guid internalId, TGraph graph, InternalSession session);
        Endure<TGraph> MakeEndure<TGraph>(TGraph graph, InternalSession session);
        Insert<TGraph> MakeInsert<TGraph>(Endure<TGraph> endure);
        Track<TGraph> MakeTrack<TGraph>(Guid internalId, TGraph graph, Stream serializedGraph, InternalSession session);
        Update<TGraph> MakeUpdate<TGraph>(Track<TGraph> track);
    }
}