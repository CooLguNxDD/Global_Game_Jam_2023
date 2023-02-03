// Copyright (C) 2016-2022 gamevanilla. All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.

using Mirror;

namespace CCGKit
{
    /// <summary>
    /// This classes manages the game scene.
    /// </summary>
    public class GameScreen : BaseScreen
    {
        public PopupChat chatPopup;

        protected override void Start()
        {
            base.Start();

            OpenPopup<PopupLoading>("PopupLoading", popup => { popup.text.text = "Waiting for game to start..."; });
        }

        public void CloseWaitingWindow()
        {
            ClosePopup();
        }

        /// <summary>
        /// Callback for when the exit game button is pressed.
        /// </summary>
        public void OnExitGameButtonPressed()
        {
            OpenPopup<PopupTwoButtons>("PopupTwoButtons", popup =>
            {
                popup.text.text = "Do you want to leave this game?";
                popup.buttonText.text = "Yes";
                popup.button2Text.text = "No";
                popup.button.onClickEvent.AddListener(() =>
                {
                    if (NetworkClient.localPlayer.isServer)
                    {
                        GameNetworkManager.Instance.StopHost();
                    }
                    else
                    {
                        GameNetworkManager.Instance.StopClient();
                    }
                });
                popup.button2.onClickEvent.AddListener(() => { popup.Close(); });
            });
        }

        /// <summary>
        /// Callback for when the chat button is pressed.
        /// </summary>
        public void OnChatButtonPressed()
        {
            chatPopup.Show();
        }
    }
}