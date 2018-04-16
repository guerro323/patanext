﻿using System;
using JetBrains.Annotations;
using P4.Default.Movements;
using Packet.Guerro.Shared.Characters;
using Packet.Guerro.Shared.Transforms;
using Unity.Entities;
using UnityEngine;

namespace P4.Default
{
    [UsedImplicitly]
    public class P4Default_MovementCoordinators : ComponentSystem
    {
        public class RequireMinimalAttribute : RequireComponentTagAttribute
        {
            public RequireMinimalAttribute()
            {
                this.TagComponents = RequireMinimal;
            }
        }

        public static Type[] RequireMinimal =
        {
            typeof(P4Default_DMovementDetailData),
            typeof(P4Default_DEntityInputUnifiedData),
            typeof(DWorldPositionData),
            typeof(DWorldRotationData),
            typeof(Rigidbody2D),
            typeof(DCharacterData),
            typeof(DCharacterInformationData),
            typeof(DCharacterCollider2DComponent)
        };
        
        protected override void OnUpdate()
        {
            
        }
    }
}