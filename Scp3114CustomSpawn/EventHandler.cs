using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;

namespace Scp3114CustomSpawn
{
    public class EventHandler
    {
        public void OnSpawning(SpawningEventArgs ev)
        {
            if(ev.Player.Role.Type == PlayerRoles.RoleTypeId.Scp3114 && (Round.ElapsedTime.TotalSeconds < 2 || MainPlugin.Instance.Config.Debug))
            {
                if(Server.PlayerCount < MainPlugin.Instance.Config.MinPlayers)
                {
                    Timing.CallDelayed(0.01f, () =>
                    {
                        ev.Player.Role.Set(MainPlugin.Instance.Config.RolesToSpawn.GetRandomValue());
                    });
                }
                else
                {
                    ev.Position = RoleTypeId.Scp173.GetRandomSpawnLocation().Position;
                }
            }
        }
    }
}
