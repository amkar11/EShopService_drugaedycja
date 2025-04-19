using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EShop.Domain.Seeders;
    public interface IEshopSeeder
    {
       Task Initialize();
    }
