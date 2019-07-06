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
