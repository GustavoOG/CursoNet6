﻿using System.Linq.Expressions;

namespace CursoNet6.AccesoDatos.Datos.Repositorio.IRepositorio
{
    public interface IRepositorio<T> where T : class
    {

        T Obtener(int id);

        IEnumerable<T> ObtenerTodos(Expression<Func<T, bool>> filtro = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string incluirPropiedades = null,
            bool isTracking = true
            );

        T ObtenerPrimero(Expression<Func<T, bool>> filtro = null,
            string incluirPropiedades = null,
            bool isTracking = true
            );

        void Agregar(T entidad);

        void Remover(T entidad);

        void Grabar();

    }
}
