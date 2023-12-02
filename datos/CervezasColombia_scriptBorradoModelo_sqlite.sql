-- Juan Dario Rodas - jdrodas@hotmail.com

-- Proyecto: Cervezas Artesanales de Colombia
-- Motor de Base de datos: SQLite

-- Script Borrado de Objetos

-- Vistas 
drop view v_info_envasados_cervezas;
drop view v_info_ingredientes;
drop view v_info_ingredientes_cervezas;
drop view v_info_cervezas;
drop view v_info_cervecerias;

-- Tablas
select ('drop table ' || name || ';') sentencia_drop
from sqlite_master
where type = 'table'
and name not like 'sqlite%';

-- Tablas temporales
drop table tmp_cervecerias;
drop table tmp_cervezas;
drop table tmp_envasados_cervezas;
drop table tmp_ingredientes;
drop table tmp_ingredientes_cervezas;

-- Tablas del dominio
drop table ingredientes_cervezas;
drop table envasados_cervezas;

drop table tipos_ingredientes;
drop table ingredientes;

drop table envasados;
drop table unidades_volumen;

drop table cervezas;
drop table cervecerias;

drop table ubicaciones;
drop table estilos;
drop table rangos_abv;
