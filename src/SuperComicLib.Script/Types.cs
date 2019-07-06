/*
Copyright 2019. Super Comic. All rights reserved.

This software was developed by the Computer Systems Engineering group
at Lawrence Berkeley Laboratory under DARPA contract BG 91-66 and
contributed to Berkeley.

All advertising materials mentioning features or use of this software
must display the following acknowledgement: This product includes
software developed by the University of California, Lawrence Berkeley
Laboratory.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are
met:

    1. Redistributions of source code must retain the above copyright
    notice, this list of conditions and the following disclaimer.

    2. Redistributions in binary form must reproduce the above copyright
    notice, this list of conditions and the following disclaimer in
    the documentation and/or other materials provided with the
    distribution.

    3. All advertising materials mentioning features or use of this
    software must display the following acknowledgement: This product
    includes software developed by the University of California,
    Berkeley and its contributors.

    4. Neither the name of the University nor the names of its
    contributors may be used to endorse or promote products derived
    from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE REGENTS AND CONTRIBUTORS ``AS IS''
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS
BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR
BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN
IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;

namespace SuperComicLib.Script
{
    public delegate void DynamicActionDelegate(object component, object value);

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public sealed class PublicSearchableAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CreateInstanceAttribute : Attribute { }

    public enum TriggerType : byte
    {
        Default = 1,
        Control = 2,
        All = byte.MaxValue
    }

    public interface IDynamicLinker
    {
        Type GetComponentType { get; }

        TriggerType AssignTo { get; }

        void EventAction(object component, object value);
    }

    public interface ITriggerControl
    {
        void SendSignal(bool ison);

        bool InputState { get; }
    }

    public sealed class GetComponentTypes
    {
        public static class None { public const string ID = nameof(None); }
        public static class GetAllEntities { public const string ID = nameof(GetAllEntities); }
        public static class GetDuplicants { public const string ID = nameof(GetDuplicants); }
        public static class GetAnimals { public const string ID = nameof(GetAnimals); }
        public static class GetGameObject { public const string ID = nameof(GetGameObject); }
    }

    public struct DynamicLinker : IDynamicLinker
    {
        public DynamicLinker(DynamicActionDelegate act) : this() => FuncAction = act;

        public DynamicLinker(DynamicActionDelegate act, Type rettype)
        {
            FuncAction = act;
            GetComponentType = rettype;
            AssignTo = TriggerType.All;
        }

        public DynamicLinker(DynamicActionDelegate act, Type rettype, TriggerType type)
        {
            FuncAction = act;
            GetComponentType = rettype;
            AssignTo = type;
        }

        public static DynamicLinker Make<T>(DynamicActionDelegate act) => new DynamicLinker(act, typeof(T));

        public static DynamicLinker Make<T>(DynamicActionDelegate act, TriggerType tp) => new DynamicLinker(act, typeof(T), tp);

        void IDynamicLinker.EventAction(object component, object value) => FuncAction.Invoke(component, value);

        public Type GetComponentType { get; private set; }
        public DynamicActionDelegate FuncAction { get; private set; }
        public TriggerType AssignTo { get; private set; }
    }
}
