using System;
using GameHost.Core.Ecs;
using GameHost.Simulation.TabEcs;
using GameHost.Simulation.TabEcs.Interfaces;
using GameHost.Simulation.Utility.EntityQuery;
using StormiumTeam.GameBase.Roles.Components;
using StormiumTeam.GameBase.Roles.Descriptions;
using StormiumTeam.GameBase.SystemBase;

namespace StormiumTeam.GameBase.GamePlay
{
	/// <summary>
	/// Get team enemies of this team
	/// </summary>
	public struct TeamEntityContainer : IComponentBuffer
	{
		public GameEntity Value;

		public TeamEntityContainer(GameEntity entity) => Value = entity;
	}

	public class BuildTeamEntityContainerSystem : GameAppSystem, IPreUpdateSimulationPass
	{
		public BuildTeamEntityContainerSystem(WorldCollection collection) : base(collection)
		{
		}

		private EntityQuery teamQuery, childQuery;

		public void OnBeforeSimulationUpdate()
		{
			var bufferAccessor = GetBufferAccessor<TeamEntityContainer>();
			foreach (var teamHandle in teamQuery ??= CreateEntityQuery(new[] {typeof(TeamEntityContainer)}))
				bufferAccessor[teamHandle].Clear();

			foreach (var child in childQuery ??= CreateEntityQuery(new[] {typeof(Relative<TeamDescription>)}))
			{
				var teamHandle = GetComponentData<Relative<TeamDescription>>(child).Handle;
				if (!teamQuery.MatchAgainst(teamHandle))
					throw new InvalidOperationException();
				bufferAccessor[teamHandle].Add(new TeamEntityContainer(Safe(child)));
			}
		}
	}
}