namespace Networking.MainGame
{
    [BoltGlobalBehaviour(BoltNetworkModes.Client,"MainGame")]
    public class ClientCallback : Bolt.GlobalEventListener
    {
        public override void Connected(BoltConnection connection)
        {
            
        }
    }
}