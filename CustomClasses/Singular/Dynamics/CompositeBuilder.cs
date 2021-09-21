#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: raphus $
// $Date: 2012-03-30 14:36:25 +0300 (Cum, 30 Mar 2012) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/Dynamics/CompositeBuilder.cs $
// $LastChangedBy: raphus $
// $LastChangedDate: 2012-03-30 14:36:25 +0300 (Cum, 30 Mar 2012) $
// $LastChangedRevision: 606 $
// $Revision: 606 $

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Singular.Managers;
using Singular.Settings;
using Styx.Combat.CombatRoutine;

using TreeSharp;

namespace Singular.Dynamics
{
    public static class CompositeBuilder
    {
        private static List<MethodInfo> _methods = new List<MethodInfo>();

        public static Composite GetComposite(WoWClass wowClass, TalentSpec spec, BehaviorType behavior, WoWContext context, out int behaviourCount)
        {
            behaviourCount = 0;
            if (_methods.Count <= 0)
            {
                Logger.Write("Building method list");
                foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    _methods.AddRange(type.GetMethods(BindingFlags.Static | BindingFlags.Public));
                }
                Logger.Write("Added " + _methods.Count + " methods");
            }
            var matchedMethods = new Dictionary<int, PrioritySelector>();
            foreach (MethodInfo mi in
                _methods.Where(
                    mi =>
                    !mi.IsGenericMethod &&
                    mi.GetParameters().Length == 0)
                    .Where(
                        mi =>
                        mi.ReturnType == typeof(Composite) ||
                        mi.ReturnType.IsSubclassOf(typeof(Composite))))
            {
                //Logger.WriteDebug("[CompositeBuilder] Checking attributes on " + mi.Name);
                bool classMatches = false, specMatches = false, behaviorMatches = false, contextMatches = false, hasIgnore = false;
                int thePriority = 0;
                var theBehaviourType = BehaviorType.All;
                var theIgnoreType = BehaviorType.All;
                foreach (object ca in mi.GetCustomAttributes(false))
                {
                    if (ca is ClassAttribute)
                    {
                        var attrib = ca as ClassAttribute;
                        if (attrib.SpecificClass != WoWClass.None && attrib.SpecificClass != wowClass)
                        {
                            continue;
                        }
                        //Logger.WriteDebug(mi.Name + " has my class");
                        classMatches = true;
                    }
                    else if (ca is SpecAttribute)
                    {
                        var attrib = ca as SpecAttribute;
                        if (attrib.SpecificSpec != TalentSpec.Any && attrib.SpecificSpec != spec)
                        {
                            continue;
                        }
                        //Logger.WriteDebug(mi.Name + " has my spec");
                        specMatches = true;
                    }
                    else if (ca is BehaviorAttribute)
                    {
                        var attrib = ca as BehaviorAttribute;
                        if ((attrib.Type & behavior) == 0)
                        {
                            continue;
                        }
                        //Logger.WriteDebug(mi.Name + " has my behavior");
                        theBehaviourType = attrib.Type;
                        behaviourCount++;
                        behaviorMatches = true;
                    }
                    else if (ca is ContextAttribute)
                    {
                        var attrib = ca as ContextAttribute;

                        if (SingularSettings.Instance.UseInstanceRotation)
                        {
                            if ((attrib.SpecificContext & WoWContext.Instances) == 0)
                                continue;
                        }
                        else if ((attrib.SpecificContext & context) == 0)
                        {
                             continue;
                        }
                        //Logger.WriteDebug(mi.Name + " has my context");
                        contextMatches = true;
                    }
                    else if (ca is PriorityAttribute)
                    {
                        var attrib = ca as PriorityAttribute;
                        thePriority = attrib.PriorityLevel;
                    }
                    else if (ca is IgnoreBehaviorCountAttribute)
                    {
                        var attrib = ca as IgnoreBehaviorCountAttribute;
                        hasIgnore = true;
                        theIgnoreType = attrib.Type;
                    }
                }

                if (behaviorMatches && hasIgnore && theBehaviourType == theIgnoreType)
                {
                    behaviourCount--;
                }

                // If all our attributes match, then mark it as wanted!
                if (classMatches && specMatches && behaviorMatches && contextMatches)
                {
                    Logger.WriteDebug("{0} is a match!", mi.Name);
                    if (!hasIgnore)
                    {
                        Logger.Write(" Using {0} for {1} - {2} (Priority: {3})", mi.Name, spec.ToString().CamelToSpaced().Trim(), behavior, thePriority);
                    }
                    else
                    {   
                        Logger.WriteDebug(" Using {0} for {1} - {2} (Priority: {3})", mi.Name, spec.ToString().CamelToSpaced().Trim(), behavior, thePriority);
                    }
                    Composite matched;
                    try
                    {
                        matched = (Composite) mi.Invoke(null, null);
                    }
                    catch (Exception e)
                    {
                        Logger.Write("ERROR Creating composite: {0}\n{1}", mi.Name, e.StackTrace);
                        continue;
                    }
                    if (!matchedMethods.ContainsKey(thePriority))
                    {
                        matchedMethods.Add(thePriority, new PrioritySelector(matched));
                    }
                    else
                    {
                        matchedMethods[thePriority].AddChild(matched);
                    }
                }
            }
            // If we found no methods, rofls!
            if (matchedMethods.Count <= 0)
            {
                return null;
            }

            var result = new PrioritySelector();
            foreach (var kvp in matchedMethods.OrderByDescending(mm => mm.Key))
            {
                result.AddChild(kvp.Value);
            }

            return result;
            // Return the composite match we found. (Note: ANY composite return is fine)
            return matchedMethods.OrderByDescending(mm => mm.Key).First().Value;
        }
    }
}