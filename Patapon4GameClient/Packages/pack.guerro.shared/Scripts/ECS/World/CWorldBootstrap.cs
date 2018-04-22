﻿using System;
using System.Linq;
using Packages.pack.guerro.shared.Scripts.Modding;
using Unity.Entities;
using UnityEditor.Compilation;
using UnityEngine;
using Assembly = System.Reflection.Assembly;

namespace Packet.Guerro.Shared.ECS
{
    public static class CWorldBootstrap
    {
        private static int m_InitCount = 0;

        private static void DomainUnloadShutdown()
        {
            World.DisposeAllWorlds();
            ScriptBehaviourUpdateOrder.UpdatePlayerLoop();
        }

        private static void GetBehaviourManagerAndLogException(World world, Type type)
        {
            try
            {
                world.GetOrCreateManager(type);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static void Init()
        {
            if (m_InitCount != 0)
            {
                Debug.LogError("CWorldBootstrap.Init() was already called.");
                throw new Exception();
            }

            m_InitCount++;

            var world = new World("Default World");
            World.Active = world;

            // Register hybrid injection hooks
            // ReSharper disable AssignNullToNotNullAttribute
            var entityAssembly = typeof(GameObjectArray).Assembly;

            InjectionHookSupport.RegisterHook
            (
                (InjectionHook) Activator.CreateInstance
                (
                    entityAssembly.GetType("Unity.Entities.GameObjectArrayInjectionHook", true)
                )
            );
            InjectionHookSupport.RegisterHook
            (
                (InjectionHook) Activator.CreateInstance
                (
                    entityAssembly.GetType("Unity.Entities.TransformAccessArrayInjectionHook")
                )
            );
            InjectionHookSupport.RegisterHook
            (
                (InjectionHook) Activator.CreateInstance
                (
                    entityAssembly.GetType("Unity.Entities.ComponentArrayInjectionHook")
                )
            );
            // ReSharper restore AssignNullToNotNullAttribute

            PlayerLoopManager.RegisterDomainUnload(DomainUnloadShutdown, 10000);

            var assembly = Assembly.GetAssembly(typeof(CWorldBootstrap));
            var allTypes = assembly.GetTypes();

            // Create all ComponentSystem
            CModManager.RegisterModInternal(new[] {assembly}, new SModInfoData()
            {
                DisplayName        = "Game Shared",
                NameId             = "pack.guerro.shared",
                IsIntegratedPacket = true,
            });
        }
    }
}