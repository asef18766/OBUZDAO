namespace Networking.MainGame
{
    [BoltGlobalBehaviour(BoltNetworkModes.Server,"MainGame")]
    public class ServerCallback : Bolt.GlobalEventListener
    {
        public override void Connected(BoltConnection connection)
        {
            PlayerRegistry.CreatePlayer(connection);
        }

        public override void Disconnected(BoltConnection connection)
        {
            PlayerRegistry.RemovePlayer(connection);
        }
    }
}