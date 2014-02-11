using System;
using System.Collections.Generic;
using FluentValidation;

namespace MtgDb.Info
{
    public class SearchModel : PageModel
    {
        public string Term { get; set; }
        public List<CardInfo> Cards { get; set; }

        public SearchModel(){
            Cards = new List<CardInfo> ();
        }
    }

    public class SearchValidator : AbstractValidator<SearchModel>
    {
        public SearchValidator()
        {
            RuleFor(search => search.Term).NotEmpty();
        }
    }
}

