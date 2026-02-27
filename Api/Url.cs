using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;

namespace Api
{
    public class Url
    {
        public int id {get; set;}
        public string? longUrl {get; set;}
        public string? shortUrl {get; set;}
        public long visits {get; set;}
    }
}