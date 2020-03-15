
using System;
using System.Collections.Generic;
using Oxide.Core;
using Oxide.Core.Plugins;
using Oxide.Core.Libraries.Covalence;
using Newtonsoft.Json;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("Information", "michaelpeterswa (nwradio)", "1.0.0")]
    [Description("Sends server information when client types /info")]
    
    class Information : CovalencePlugin
    {
        #region Config

        private static ConfigurationData config;

        private class ConfigurationData
        {
            [JsonProperty(PropertyName = "InformationMessage")]
            public String InformationMessage;
            
            [JsonProperty(PropertyName = "ShowIP")]
            public bool ShowIP;

            [JsonProperty(PropertyName = "ShowPing")]
            public bool ShowPing;

            [JsonProperty(PropertyName = "SecondaryMessage")]
            public String SecondaryMessage;

            [JsonProperty(PropertyName = "RulesTitle")]
            public String RulesTitle;

            [JsonProperty(PropertyName = "RulesText")]
            public String RulesText;

            [JsonProperty(PropertyName = "AdminTitle")]
            public String AdminTitle;

            [JsonProperty(PropertyName = "AdminText")]
            public String AdminText;

            [JsonProperty(PropertyName = "CommandTitle")]
            public String CommandTitle;

            [JsonProperty(PropertyName = "CommandText")]
            public String CommandText;
        }

        private ConfigurationData GetDefaultConfig()
        {
            return new ConfigurationData
            {
                InformationMessage = "<color=red>Here is the default message!</color>",
                ShowIP = true,
                ShowPing = true,
                SecondaryMessage = "Rules: /info rules \nAdmins: /info admin \nCommands: /info cmd",
                RulesTitle = "<color=red>Server Rules:</color>",
                RulesText = "Have Fun!",
                AdminTitle = "<color=red>Admins:</color>",
                AdminText = "list admin/owner here",
                CommandTitle = "<color=red>Commands:</color>",
                CommandText = "list commands here"
            };
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();

            try
            {
                config = Config.ReadObject<ConfigurationData>();

                if (config == null)
                {
                    LoadDefaultConfig();
                }
            }
            catch
            {
                PrintError("Configuration file is corrupt! Unloading plugin...");
                Interface.Oxide.RootPluginManager.RemovePlugin(this);
                return;
            }

            SaveConfig();
        }

        protected override void LoadDefaultConfig()
        {
            config = GetDefaultConfig();
        }

        protected override void SaveConfig()
        {
            Config.WriteObject(config);
        }

        #endregion

        #region CommandHandler
        [Command("info")]
        private void PrintCommand(IPlayer player, string command, string[] args)
        {
            if(args.Length == 0){
                string ip = player.Address;
                string ping = (player.Ping).ToString();

                string message = config.InformationMessage;
                
                if(config.ShowIP){
                    message = message + "\n" + "IP: " + ip;
                }
                if(config.ShowPing){
                    message = message + "\n" + "Ping: " + ping + "ms";
                }

                message = message + "\n" + config.SecondaryMessage;
                player.Reply(message);
            }
            
            if(args.Length == 1 && args[0] == "rules"){
                string rulesmessage = config.RulesTitle;
                rulesmessage = rulesmessage + "\n" + config.RulesText;

                player.Reply(rulesmessage);
            }
            
            if(args.Length == 1 && args[0] == "admin"){
                string adminmessage = config.AdminTitle;
                adminmessage = adminmessage + "\n" + config.AdminText;
                
                player.Reply(adminmessage);
            }

            if(args.Length == 1 && args[0] == "cmd"){
                string commandmessage = config.CommandTitle;
                commandmessage = commandmessage + "\n" + config.CommandText;
                
                player.Reply(commandmessage);
            }
        }

        #endregion
    }

}