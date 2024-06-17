using Oxide.Core.Plugins;
using Oxide.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("MailBomber", "herbs.acab", "1.0.5")]
    [Description("Interacting with a mailbox causes an MLRS rocket barrage to activate.")]
    public class MailBomber : RustPlugin
    {
        private const string permissionUse = "mailbomber.use";
        private List<BaseEntity> createdEntities = new List<BaseEntity>();
        private List<string> noteMessages = new List<string>();

        private void Init()
        {
            permission.RegisterPermission(permissionUse, this);
            LoadConfig();
        }

        protected override void LoadDefaultConfig()
        {
            Config["NoteMessages"] = new List<string>
            {
                "Unfortunately due to the crimes you have committed, we will be executing you on the spot.",
                "This is a second configurable message."
            };
            SaveConfig();
        }

        private void LoadConfig()
        {
            noteMessages = Config.Get<List<string>>("NoteMessages");
        }

        [ChatCommand("mailbomber")]
        private void MailBomberCommand(BasePlayer player, string command, string[] args)
        {
            if (!HasPermission(player))
            {
                SendReply(player, "You don't have permission to use this command.");
                return;
            }

            RaycastHit hit;
            if (!Physics.Raycast(player.eyes.HeadRay(), out hit))
            {
                SendReply(player, "No mailbox found.");
                return;
            }

            var mailbox = hit.GetEntity();
            if (mailbox == null || mailbox.ShortPrefabName != "mailbox.deployed")
            {
                SendReply(player, "You must look at a mailbox to use this command.");
                return;
            }

            mailbox.gameObject.AddComponent<MailBomberMailbox>();
            createdEntities.Add(mailbox);
            AddNotesToMailbox(mailbox);
            SendReply(player, "MailBomber has been activated on this mailbox.");
        }

        [ChatCommand("clearmailbomber")]
        private void ClearMailBomberCommand(BasePlayer player, string command, string[] args)
        {
            if (!HasPermission(player))
            {
                SendReply(player, "You don't have permission to use this command.");
                return;
            }

            foreach (var entity in createdEntities)
            {
                if (entity != null && !entity.IsDestroyed)
                {
                    var component = entity.GetComponent<MailBomberMailbox>();
                    if (component != null)
                    {
                        GameObject.Destroy(component);
                    }
                }
            }

            createdEntities.Clear();
            SendReply(player, "All MailBomber entities have been cleaned up.");
        }

        private bool HasPermission(BasePlayer player)
        {
            return permission.UserHasPermission(player.UserIDString, permissionUse);
        }

        private void OnEntityKill(BaseNetworkable entity)
        {
            if (createdEntities.Contains(entity as BaseEntity))
            {
                createdEntities.Remove(entity as BaseEntity);
            }
        }

        private void AddNotesToMailbox(BaseEntity mailbox)
        {
            foreach (var message in noteMessages)
            {
                var noteItem = ItemManager.Create(ItemManager.FindItemDefinition("note"), 1);
                if (noteItem != null)
                {
                    noteItem.text = message;
                    noteItem.MarkDirty();

                    var storage = mailbox as StorageContainer;
                    if (storage != null)
                    {
                        storage.inventory.Insert(noteItem);
                    }
                }
            }
        }

        private void OnLootEntity(BasePlayer player, BaseEntity entity)
        {
            if (entity.ShortPrefabName == "mailbox.deployed")
            {
                if (!player.IPlayer.HasPermission(permissionUse))
                {
                    player.ChatMessage("You don't have permission to interact with this mailbox.");
                    return;
                }

                var position = entity.transform.position;
                foreach (var activePlayer in BasePlayer.activePlayerList)
                {
                    Effect.server.Run("assets/prefabs/npc/mlrs/rocket_mlrs/effects/mlrsrocket_explosion.prefab", position);
                }
                Debug.Log("MLRS Rocket Barrage deployed.");
            }
        }

        private class MailBomberMailbox : MonoBehaviour
        {
            private BaseEntity entity;

            private void Awake()
            {
                entity = GetComponent<BaseEntity>();
            }
        }
    }
}
