// =============================
// Email: info@ebenmonney.com
// www.ebenmonney.com/templates
// =============================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;
using DAL.Repositories.Interfaces;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext _context;

        // IProductRepository _products;



        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }






        // public IProductRepository Products
        // {
        //     get
        //     {
        //         if (_products == null)
        //             _products = new ProductRepository(_context);

        //         return _products;
        //     }
        // }





        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
