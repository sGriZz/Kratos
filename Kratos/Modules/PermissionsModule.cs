﻿using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Discord.WebSocket;
using Discord.Commands;
using Kratos.Preconditions;
using Kratos.Services;
using Kratos.Services.Results;

namespace Kratos.Modules
{
    [Name("Permissions Module"), Group("perms")]
    [Summary("Manage the bot's permission system.")]
    public class PermissionsModule : ModuleBase
    {
        private PermissionsService _service;

        [Command("add"), Alias("+")]
        [Summary("Add a permission to a role.")]
        [RequireCustomPermission("permissions.manage")]
        public async Task AddPerm([Summary("Role to which to add the permission")] SocketRole role,
                                  [Summary("Permission to be added")] string permission)
        {
            var result = _service.AddPermission(role, permission);
            switch (result.Type)
            {
                case ResultType.Success:
                    await _service.SaveConfigurationAsync();
                    await ReplyAsync($":ok: {result.Message}");
                    break;
                case ResultType.Warning:
                    await ReplyAsync($":warning: {result.Message}");
                    break;
                case ResultType.Fail:
                    await ReplyAsync($":x: {result.Message}");
                    break;
            }
        }

        [Command("remove"), Alias("-")]
        [Summary("Removes a permission from a role.")]
        [RequireCustomPermission("permissions.manage")]
        public async Task RemovePerm([Summary("Role from which to remove the permission")] SocketRole role,
                                     [Summary("Permission to removed")] string permission)
        {
            var result = _service.RemovePermission(role, permission);
            switch (result.Type)
            {
                case ResultType.Success:
                    await _service.SaveConfigurationAsync();
                    await ReplyAsync($":ok: {result.Message}");
                    break;
                case ResultType.Warning:
                    await ReplyAsync($":warning: {result.Message}");
                    break;
                case ResultType.Fail:
                    await ReplyAsync($":x: {result.Message}");
                    break;
            }
        }

        [Command("list")]
        [Summary("Lists all permissions held by a role.")]
        [RequireCustomPermission("permissions.view")]
        public async Task ListPerms([Summary("Role for which to list permissions")] SocketRole role)
        {
            var response = new StringBuilder($"**Permissions held by {role.Name}**:\n\n");
            var permissions = _service.GetPermissionsForRole(role);
            if (permissions != null)
            {
                foreach (var p in _service.Permissions[role.Id].OrderBy(p => p))
                    response.AppendLine(p);
            }
            await ReplyAsync(response.ToString());
        }

        [Command("listall")]
        [Summary("Lists all existing permissions.")]
        [RequireCustomPermission("permissions.view")]
        public async Task ListAll()
        {
            var response = new StringBuilder("__All permissions__\n\n");
            foreach (var p in _service.AllPermissions.OrderBy(x => x))
                response.AppendLine(p);
            await ReplyAsync(response.ToString());
        }

        [Command("saveconfig"), Alias("save")]
        [Summary("Saves the current permission configuration")]
        [RequireCustomPermission("permissions.manage")]
        public async Task SaveConfig()
        {
            var success = await _service.SaveConfigurationAsync();
            await ReplyAsync(success ? ":ok:" : ":x: Failed to save config.");
        }

        [Command("reloadconfig"), Alias("reload")]
        [Summary("Reloads the permission configuration from config\\permissions.json")]
        [RequireCustomPermission("permissions.manage")]
        public async Task ReloadConfig()
        {
            var success = await _service.LoadConfigurationAsync();
            await ReplyAsync(success ? ":ok:" : ":x: Failed to reload config. Please configure permissions and save the config.");
        }

        public PermissionsModule(PermissionsService p)
        {
            _service = p;
        }
    }
}
