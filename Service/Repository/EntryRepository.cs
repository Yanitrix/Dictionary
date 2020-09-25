﻿using Data.Database;
using Data.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Service
{
    public class EntryRepository : RepositoryBase<Entry>, IEntryRepository
    {
        public EntryRepository(DatabaseContext context):base(context) { }

        //so basically include everything
        private readonly Func<IQueryable<Entry>, IIncludableQueryable<Entry, object>> includeQuery =
            (entries => entries.Include(e => e.Word).Include(e => e.Meanings).ThenInclude(m => m.Examples));

        public IEnumerable<Entry> GetByDictionaryAndWord(int dictionaryIndex, string wordValue)
        {
            if (String.IsNullOrEmpty(wordValue) || String.IsNullOrWhiteSpace(wordValue)) return Enumerable.Empty<Entry>();
            return Get(e => e.DictionaryIndex == dictionaryIndex && EF.Functions.Like(e.Word.Value, $"%{wordValue}%"), x => x, null, includeQuery); //ignore case
        }

        public Entry GetByID(int id)
        {
            return GetOne(e => e.ID == id, null, includeQuery);
        }

    }
}
