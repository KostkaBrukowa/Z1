using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Z01.Models
{
  public class NoteFilterModel
  {
    private int _page;

    public NoteFilterModel(NoteFilterModel other)
    {
      From = other.From;
      To = other.To;
      SelectedCategory = other.SelectedCategory;
      Page = other.Page;
      Error = other.Error;
    }

    public NoteFilterModel(DateTime from, DateTime to, string category, int page, string error)
    {
      From = from == new DateTime() ? DateTime.Now.AddDays(-7) : from;
      To = to == new DateTime() ? DateTime.Now.AddDays(1) : to;
      SelectedCategory = category;
      Page = page < 0 ? 0 : page;
      Error = error;
    }

    public NoteFilterModel() : this(new DateTime(), new DateTime(), null, 0, null)
    {
    }


    [DataType(DataType.Date)] public DateTime From { get; set; }
    [DataType(DataType.Date)] public DateTime To { get; set; }
    public string SelectedCategory { get; set; }
    public string Error { get; set; }

    [HiddenInput]
    public int Page
    {
      get => _page;
      set => _page = value < 0 ? 0 : value;
    }

    public NoteFilterModel TrimPages(int maxPage)
    {
      if (Page > maxPage) Page = maxPage;

      return this;
    }
  }
}
