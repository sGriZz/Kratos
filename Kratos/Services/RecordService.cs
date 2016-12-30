﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Kratos.Data;

namespace Kratos.Services
{
    public class RecordService
    {
        private RecordContext _db;

        public async Task<Mute> AddMuteAsync(
            ulong guildId,
            ulong subjectId,
            ulong moderatorId,
            ulong timestamp,
            ulong unMuteAtTimestamp,
            string reason)
        {
            if (_db == null)
                _db = new RecordContext();
            await _db.Database.EnsureCreatedAsync();
            var mute = await _db.Mutes.AddAsync(new Mute
            {
                Active = true,
                GuildId = guildId,
                SubjectId = subjectId,
                ModeratorId = moderatorId,
                UnixTimestamp = timestamp,
                UnmuteAtUnixTimestamp = unMuteAtTimestamp,
                Reason = reason
            });

            await _db.SaveChangesAsync();
            return mute.Entity;
        }

        public async Task<PermaBan> AddPermaBanAsync(
            ulong guildId,
            ulong subjectId,
            string subjectName,
            ulong moderatorId,
            ulong timestamp,
            string reason)
        {
            if (_db == null)
                _db = new RecordContext();
            await _db.Database.EnsureCreatedAsync();
            var ban = await _db.PermaBans.AddAsync(new PermaBan
            {
                GuildId = guildId,
                SubjectId = subjectId,
                SubjectName = subjectName,
                ModeratorId = moderatorId,
                UnixTimestamp = timestamp,
                Reason = reason
            });

            await _db.SaveChangesAsync();
            return ban.Entity;
        }

        public async Task<TempBan> AddTempBanAsync(
            ulong guildId,
            ulong subjectId,
            string subjectName,
            ulong moderatorId,
            ulong timestamp,
            ulong unBanAtTimestamp,
            string reason)
        {
            if (_db == null)
                _db = new RecordContext();
            await _db.Database.EnsureCreatedAsync();
            var ban = await _db.TempBans.AddAsync(new TempBan
            {
                Active = true,
                GuildId = guildId,
                SubjectId = subjectId,
                SubjectName = subjectName,
                ModeratorId = moderatorId,
                UnixTimestamp = timestamp,
                UnbanAtUnixTimestamp = unBanAtTimestamp,
                Reason = reason
            });

            await _db.SaveChangesAsync();
            return ban.Entity;
        }

        public async Task<SoftBan> AddSoftBanAsync(
            ulong guildId,
            ulong subjectId,
            string subjectName,
            ulong moderatorId,
            ulong timestamp,
            string reason)
        {
            if (_db == null)
                _db = new RecordContext();
            await _db.Database.EnsureCreatedAsync();
            var ban = await _db.SoftBans.AddAsync(new SoftBan
            {
                GuildId = guildId,
                SubjectId = subjectId,
                SubjectName = subjectName,
                ModeratorId = moderatorId,
                UnixTimestamp = timestamp,
                Reason = reason
            });

            await _db.SaveChangesAsync();
            return ban.Entity;
        }

        public async Task DeactivateBanAsync(int key)
        {
            if (_db == null)
                _db = new RecordContext();
            await _db.Database.EnsureCreatedAsync();
            var ban = await _db.TempBans.FirstOrDefaultAsync(x => x.Key == key);
            ban.Active = false;
            await _db.SaveChangesAsync();
        }

        public async Task DeactivateMuteAsync(int key)
        {
            if (_db == null)
                _db = new RecordContext();
            await _db.Database.EnsureCreatedAsync();
            var mute = await _db.Mutes.FirstOrDefaultAsync(x => x.Key == key);
            mute.Active = false;
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TempBan>> GetActiveTempBansAsync()
        {
            if (_db == null)
                _db = new RecordContext();
            await _db.Database.EnsureCreatedAsync();
            return _db.TempBans.Where(x => DateTime.Compare(DateTime.UtcNow, new DateTime(1970, 1, 1).AddSeconds(x.UnbanAtUnixTimestamp)) > 0 && x.Active);
        }

        public async Task<IEnumerable<Mute>> GetActiveMutesAsync()
        {
            if (_db == null)
                _db = new RecordContext();
            await _db.Database.EnsureCreatedAsync();
            return _db.Mutes.Where(x => DateTime.Compare(DateTime.UtcNow, new DateTime(1970, 1, 1).AddSeconds(x.UnmuteAtUnixTimestamp)) > 0 && x.Active);
        }

        public void DisposeContext()
        {
            _db.Dispose();
            _db = null;
        }
    }
}