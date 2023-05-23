﻿using System;
using System.Collections.Generic;

namespace ArenaGestor.APIContracts.Band
{
    public class BandInsertBandDto
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public int GenderId { get; set; }

        public List<BandInsertArtistDto> Artists { get; set; }
    }
}
