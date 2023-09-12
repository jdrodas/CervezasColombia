-- Scripts de clase - Septiembre 12 de 2023
-- Curso de Tópicos Avanzados de base de datos - UPB 202320
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Cervezas Artesanales de Colombia
-- Motor de Base de datos: SQLite

-- Script Borrado de Objetos

-- Vistas 

drop view v_info_envasados_cervezas;
drop view v_info_ingredientes;
drop view v_info_ingredientes_cervezas;
drop view v_info_cervezas;

-- Tablas

select ('drop table ' || name || ';') sentencia_drop
from sqlite_master
where type = 'table'
and name not like 'sqlite%';

drop table cervezas_dg_tmp;
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
drop table rangos_ibu;

