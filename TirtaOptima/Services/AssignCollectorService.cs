using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
	public class AssignCollectorService(DatabaseContext context)
	{
		private readonly DatabaseContext _context = context;
		public List<AssignColumn> GetItems(string[] itemjson)
		{
			var result = new List<AssignColumn>();
			var policy = _context.ActionTypes.ToList();
			foreach (var json in itemjson)
			{
				var item = JsonConvert.DeserializeObject<ResultColumn>(json);
				if (item != null)
				{
					result.Add(new AssignColumn
					{
						Id = item.PiutangId,
						Policy = policy.FirstOrDefault(x => x.Name == item.Tindakan)?.Id
					});
				}
			}

			return result;
		}

		public void Save(StrategyResultViewModel input, long userid)
		{
			var newCollections = new List<Collection>();
			var lastId = _context.Collections.Any()
			? _context.Collections.Max(x => x.Id)
			: 0;
			foreach (var item in input.SelectedItems)
			{
				lastId++; 

				newCollections.Add(new Collection
				{
					Id = lastId,
					PiutangId = item.Id,
					TindakanId = item.Policy,
					StatusId = "Belum Ditagih",
					PenagihId = input.Penagih,
					Tanggal = DateTime.Now,
					CreatedAt = DateTime.Now,
					CreatedBy = userid,
					UpdatedAt = DateTime.Now,
					UpdatedBy = userid
				});
				var existing = _context.StrategyResults.FirstOrDefault(x => x.PiutangId == item.Id);
				if (existing != null)
				{
					existing.Status = "diterapkan";
					existing.UpdatedAt = DateTime.Now;
					existing.UpdatedBy = userid;
				}
			}
			_context.Collections.AddRange(newCollections);
			_context.SaveChanges();
		}
	}
}
