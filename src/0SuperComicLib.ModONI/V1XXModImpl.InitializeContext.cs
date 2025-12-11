// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace SuperComicLib.ModONI;

partial class V1XXModImpl
{
    [StructLayout(LayoutKind.Sequential)]
    protected unsafe ref struct InitializeContext
    {
        private IntPtr _dummy_dereference;
        
        internal void MakeLoopReference()
        {
            var p = (IntPtr*)Unsafe.AsPointer(ref _dummy_dereference);
            // make loop reference
            *p = (IntPtr)p;
        }

        public readonly ref readonly ModUtilAccessor Utils
        {
            get
            {
                System.Diagnostics.Debug.Assert(_dummy_dereference != IntPtr.Zero);
                return ref *(ModUtilAccessor*)_dummy_dereference;
            }
        }

        public readonly ref readonly ModTechAccessor Techs
        {
            get
            {
                System.Diagnostics.Debug.Assert(_dummy_dereference != IntPtr.Zero);
                return ref *(ModTechAccessor*)_dummy_dereference;
            }
        }
    }
    
    [StructLayout(LayoutKind.Sequential)]
    protected readonly unsafe ref struct ModUtilAccessor
    {
        private readonly void* _reference;
        
        public readonly ref readonly ModUtilAccessor Base(params ReadOnlySpan<string> value) => ref AddBuildingToPlanScreen(nameof(Base), value);
        public readonly ref readonly ModUtilAccessor Oxygen(params ReadOnlySpan<string> value) => ref AddBuildingToPlanScreen(nameof(Oxygen), value);
        public readonly ref readonly ModUtilAccessor Power(params ReadOnlySpan<string> value) => ref AddBuildingToPlanScreen(nameof(Power), value);
        public readonly ref readonly ModUtilAccessor Food(params ReadOnlySpan<string> value) => ref AddBuildingToPlanScreen(nameof(Food), value);
        public readonly ref readonly ModUtilAccessor Plumbing(params ReadOnlySpan<string> value) => ref AddBuildingToPlanScreen(nameof(Plumbing), value);
        public readonly ref readonly ModUtilAccessor HVAC(params ReadOnlySpan<string> value) => ref AddBuildingToPlanScreen(nameof(HVAC), value);
        public readonly ref readonly ModUtilAccessor Refining(params ReadOnlySpan<string> value) => ref AddBuildingToPlanScreen(nameof(Refining), value);
        public readonly ref readonly ModUtilAccessor Medical(params ReadOnlySpan<string> value) => ref AddBuildingToPlanScreen(nameof(Medical), value);
        public readonly ref readonly ModUtilAccessor Furniture(params ReadOnlySpan<string> value) => ref AddBuildingToPlanScreen(nameof(Furniture), value);
        public readonly ref readonly ModUtilAccessor Equipment(params ReadOnlySpan<string> value) => ref AddBuildingToPlanScreen(nameof(Equipment), value);
        public readonly ref readonly ModUtilAccessor Utilities(params ReadOnlySpan<string> value) => ref AddBuildingToPlanScreen(nameof(Utilities), value);
        public readonly ref readonly ModUtilAccessor Automation(params ReadOnlySpan<string> value) => ref AddBuildingToPlanScreen(nameof(Automation), value);
        public readonly ref readonly ModUtilAccessor Conveyance(params ReadOnlySpan<string> value) => ref AddBuildingToPlanScreen(nameof(Conveyance), value);
        public readonly ref readonly ModUtilAccessor Rocketry(params ReadOnlySpan<string> value) => ref AddBuildingToPlanScreen(nameof(Rocketry), value);
        public readonly ref readonly ModUtilAccessor HEP(params ReadOnlySpan<string> value) => ref AddBuildingToPlanScreen(nameof(HEP), value);

        public readonly ref readonly ModUtilAccessor AddBuildingToPlanScreen(HashedString category, params ReadOnlySpan<string> buildingIDs)
        {
            for (int i = 0; i < buildingIDs.Length; ++i)
            {
                ModUtil.AddBuildingToPlanScreen(category, buildingIDs[i]);
            }
            
            // trick (loop reference)
            return ref *(ModUtilAccessor*)_reference;
        }
    }
    
    [StructLayout(LayoutKind.Sequential)]
    protected readonly unsafe ref struct ModTechAccessor
    {
        private readonly void* _reference;

        public readonly ref readonly ModTechAccessor AddTo(WellKnownTechId techId, params ReadOnlySpan<string> buildingIDs) => ref ReserveAddToTechOn(techId.ToString(), buildingIDs);
        public readonly ref readonly ModTechAccessor AddTo(string techId, params ReadOnlySpan<string> buildingIDs) => ref ReserveAddToTechOn(techId, buildingIDs);

        private readonly ref readonly ModTechAccessor ReserveAddToTechOn(string techId, ReadOnlySpan<string> buildingIDs)
        {
            var list = ModTechs.LazyTemporaryList;
            for (int i = 0; i < buildingIDs.Length; ++i)
            {
                list.Add((techId, buildingIDs[i]));
            }

            // trick (loop reference)
            return ref *(ModTechAccessor*)_reference;
        }
    }
}