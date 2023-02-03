// Copyright (C) 2016-2022 gamevanilla. All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.

using Mirror;

namespace CCGKit
{
    /// <summary>
    /// This server handler is responsible for managing the in-game chat.
    /// </summary>
    public class ChatHandler : ServerHandler
    {
        public ChatHandler(Server server) : base(server)
        {
        }

        public override void RegisterNetworkHandlers()
        {
            base.RegisterNetworkHandlers();
            NetworkServer.RegisterHandler<SendChatTextMessage>(OnSendChatText);
        }

        public override void UnregisterNetworkHandlers()
        {
            base.UnregisterNetworkHandlers();
            NetworkServer.UnregisterHandler<SendChatTextMessage>();
        }

        public virtual void OnSendChatText(NetworkConnection conn, SendChatTextMessage msg)
        {
            var player = server.gameState.players.Find(x => x.netId == msg.senderNetId);
            if (player != null)
            {
                var broadcastMsg = new BroadcastChatTextMessage();
                broadcastMsg.senderNetId = msg.senderNetId;
                broadcastMsg.text = player.nickname + ": " + msg.text;
                NetworkServer.SendToAll<BroadcastChatTextMessage>(broadcastMsg);
            }
        }
    }
}
