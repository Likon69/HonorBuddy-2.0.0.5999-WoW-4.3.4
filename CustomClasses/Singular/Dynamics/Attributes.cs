#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: exemplar $
// $Date: 2011-05-06 08:40:00 +0300 (Cum, 06 May 2011) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/Dynamics/Attributes.cs $
// $LastChangedBy: exemplar $
// $LastChangedDate: 2011-05-06 08:40:00 +0300 (Cum, 06 May 2011) $
// $LastChangedRevision: 310 $
// $Revision: 310 $

#endregion

using System;

using Singular.Managers;

using Styx.Combat.CombatRoutine;

namespace Singular.Dynamics
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    internal sealed class PriorityAttribute : Attribute
    {
        public PriorityAttribute(int thePriority)
        {
            PriorityLevel = thePriority;
        }

        public int PriorityLevel { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    internal sealed class ClassAttribute : Attribute
    {
        public ClassAttribute(WoWClass specificClass)
        {
            SpecificClass = specificClass;
        }

        public WoWClass SpecificClass { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    internal sealed class IgnoreBehaviorCountAttribute : Attribute
    {
        public IgnoreBehaviorCountAttribute(BehaviorType type)
        {
            Type = type;
        }

        public BehaviorType Type { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    internal sealed class BehaviorAttribute : Attribute
    {
        public BehaviorAttribute(BehaviorType type)
        {
            Type = type;
        }

        public BehaviorType Type { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    internal sealed class SpecAttribute : Attribute
    {
        public SpecAttribute(TalentSpec spec)
        {
            SpecificSpec = spec;
        }

        public TalentSpec SpecificSpec { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    internal sealed class ContextAttribute : Attribute
    {
        public ContextAttribute(WoWContext context)
        {
            SpecificContext = context;
        }

        public WoWContext SpecificContext { get; private set; }
    }
}