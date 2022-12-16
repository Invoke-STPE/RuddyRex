﻿using RuddyRex.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuddyRex.Lib.Models.TokenModels
{
    public record TokenString : IToken
    {
        public TokenType Type { get; } = TokenType.StringLiteral;
        public string Value { get; set; }
    }
}
