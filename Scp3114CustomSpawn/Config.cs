using Exiled.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using System.ComponentModel;

namespace Scp3114CustomSpawn
{
   public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Enabled Debug will remove round timer checking")]
        public bool Debug { get; set; } = false;

        [Description("Min players to spawn for SCP-3114")]
        public int MinPlayers { get; set; } = 9;

        [Description("Roles that will randomly choosed for skeleton, when players count is not enough")]
        public List<RoleTypeId> RolesToSpawn { get; set; } = new List<RoleTypeId>()
        { 
          RoleTypeId.ClassD,
          RoleTypeId.Scientist
        };
    }
}
