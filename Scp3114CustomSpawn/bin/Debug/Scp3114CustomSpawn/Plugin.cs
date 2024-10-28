using Exiled.API.Features;

namespace Scp3114CustomSpawn 
{
    public class MainPlugin : Plugin<Config>
    {
        public static MainPlugin Instance { get; private set; }

        public EventHandler eventHandler;
        public override void OnEnabled()
        {
            Instance = this;
            eventHandler = new EventHandler();
            Exiled.Events.Handlers.Player.Spawning += eventHandler.OnSpawning;
        }
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Spawning += eventHandler.OnSpawning;
        }
    }
}
