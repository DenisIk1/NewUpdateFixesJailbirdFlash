using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Spawn;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using PlayerRoles.FirstPersonControl.Spawnpoints;
using System.Linq;

namespace Scp3114CustomSpawn
{
    public class EventHandler
    {
        public void OnSpawning(SpawningEventArgs ev)
        {
            if(ev.Player.Role.Type == PlayerRoles.RoleTypeId.Scp3114)
            {
                ev.Position = RoleTypeId.Scp173.GetRandomSpawnLocation().Position;
                if ((Round.ElapsedTime.TotalSeconds < 2 || MainPlugin.Instance.Config.Debug) && Server.PlayerCount < MainPlugin.Instance.Config.MinPlayers)
                {
                    Timing.CallDelayed(0.01f, () =>
                    {
                        ev.Player.Role.Set(MainPlugin.Instance.Config.RolesToSpawn.GetRandomValue());
                    });
                }
            }
        }
    }
}
