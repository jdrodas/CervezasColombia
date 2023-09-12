-- Scripts de clase - Septiembre 9 de 2023
-- Curso de Tópicos Avanzados de base de datos - UPB 202320
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Cervezas Artesanales de Colombia
-- Motor de Base de datos: SQLite

-- Script de creación de tablas y vistas

-- Tablas

create table ubicaciones
(
    id           integer constraint ubicaciones_pk primary key autoincrement,
    municipio    text not null,
    departamento text not null,
    constraint ubicaciones_uk unique (municipio, departamento)
);

create table cervecerias
(
    id           integer constraint cervecerias_pk primary key autoincrement,
    nombre       text not null,
    ubicacion_id integer not null constraint cerveceria_ubicacion_fk references ubicaciones,
    sitio_web     text not null,
    instagram    integer not null,
	constraint cerveceria_nombre_uk unique (nombre),
	constraint cerveceria_sitioweb_uk unique (sitio_web),
	constraint cerveceria_instagram_uk unique (instagram)
);

create table estilos
(
	id           integer constraint estilos_pk primary key autoincrement,
    nombre       text not null,
	constraint estilo_nombre_uk unique (nombre)    
);

create table cervezas
(
	id           integer constraint cervezas_pk primary key autoincrement,
    nombre       text not null,
    estilo_id     integer not null constraint cerveza_estilo_fk references estilos,
    cerveceria_id integer not null constraint cerveza_cerveceria_fk references cervecerias,
    ibu           real not null,
    abv           real not null,
	constraint cervezas_uk unique (nombre,estilo_id,cerveceria_id)
);

create table rangos_abv
(
    id            integer constraint rangos_abv_pk  primary key autoincrement,
    nombre        text not null,
    valor_inicial real not null,
    valor_final   real not null,
	constraint rangos_abv_nombre_uk unique (nombre)
);

create table rangos_ibu
(
    id            integer constraint rangos_ibu_pk  primary key autoincrement,
    nombre        text not null,
    valor_inicial real not null,
    valor_final   real not null,
	constraint rangos_ibu_nombre_uk unique (nombre)
);

create table envasados
(
    id            integer constraint envasados_pk  primary key autoincrement,
    nombre        text not null,
	constraint envasados_nombre_uk unique (nombre)	
);

create table unidades_volumen
(
    id            integer constraint unidades_volumen_pk  primary key autoincrement,
    nombre        text not null,
    abreviatura text not null,
	constraint unidades_volumen_uk unique (nombre,abreviatura),
	constraint unidades_volumen_nombre_uk unique (nombre),	
	constraint unidades_volumen_abreviatura_uk unique (abreviatura)
);

create table envasados_cervezas
(
    cerveza_id        integer not null constraint envasados_cervezas_cerveza_fk references cervezas,
    envasados_id      integer not null constraint envasados_cervezas_envasado_fk references envasados,
    unidad_volumen_id integer not null constraint envasados_cervezas_unidad_volumen_fk references unidades_volumen,
    volumen           integer,
    constraint envasados_cervezas_pk primary key (cerveza_id, envasados_id, unidad_volumen_id, volumen)
);

create table tipos_ingredientes
(
    id            integer constraint tipos_ingredientes_pk  primary key autoincrement,
    nombre        text not null,
	constraint tipos_ingredientes_uk unique (nombre)	
);

create table ingredientes
(
    id                  integer constraint ingredientes_fk primary key autoincrement,
    nombre              text    not null,
    tipo_ingrediente_id integer not null constraint ingrediente_tipo_ingrediente_fk references tipos_ingredientes,
	constraint ingrediente_tipo_ingrediente_uk unique (nombre,tipo_ingrediente_id)
);

create table ingredientes_cervezas
(
    cerveza_id     integer not null constraint ingredientesCerveza_cerveza_Fk references cervezas,
    ingrediente_id integer not null constraint ingredientesCerveza_ingredientes_fk references ingredientes,
    constraint ingredientes_cervezas_pk primary key (cerveza_id, ingrediente_id)
);

-- Vistas:

create view v_info_cervezas as
select
    cz.id cerveza_id,
    cz.nombre cerveza,
    c.id cerveceria_id,
    c.nombre cerveceria,
    e.id estilo_id,
    e.nombre estilo,
    cz.ibu,
    ri.nombre rango_ibu,
    cz.abv,
    ra.nombre rango_abv
from cervezas cz
    join cervecerias c on cz.cerveceria_id = c.id
    join estilos e on cz.estilo_id = e.id,
rangos_abv ra, rangos_ibu ri
where cz.ibu between ri.valor_inicial and ri.valor_final
    and cz.abv between ra.valor_inicial and ra.valor_final;
	
create view v_info_envasados_cervezas as
select
    cr.id cerveceria_id,
    cr.nombre cerveceria,
    ec.cerveza_id,
    c.nombre cerveza,
    ec.envasados_id,
    e.nombre envasado,
    ec.unidad_volumen_id,
    uv.nombre unidad_volumen,
    ec.volumen
from envasados_cervezas ec
    join cervezas c on ec.cerveza_id = c.id
    join cervecerias cr on c.cerveceria_id = cr.id
    join envasados e on ec.envasados_id = e.id
    join unidades_volumen uv on ec.unidad_volumen_id = uv.id;

create view v_info_ingredientes as
select
    ti.id tipo_ingrediente_id,
    ti.nombre tipo_ingrediente,
    i.id ingrediente_id,
    i.nombre ingrediente
    from ingredientes i
    join tipos_ingredientes ti on i.tipo_ingrediente_id = ti.id;
	
create view v_info_ingredientes_cervezas as
select
    c.id cerveceria_id,
    c.nombre cerveceria,
    cv.id cerveza_id,
    cv.nombre cerveza,
    ti.id tipo_ingrediente_id,
    ti.nombre tipo_ingrediente,
    i.id ingrediente_id,
    i.nombre ingrediente
from ingredientes_cervezas ic
    join cervezas cv on cv.id = ic.cerveza_id
    join cervecerias c on cv.cerveceria_id = c.id
    join ingredientes i on i.id = ic.ingrediente_id
    join tipos_ingredientes ti on i.tipo_ingrediente_id = ti.id;	

-- *****************************
-- Orden de cargue de datos
-- *****************************

/*
- rangos_abv
- rango_ibu
- estilos
- ubicaciones
- cervecerias
- cervezas
- envasados
- unidades_volumen
- envasados_cervezas
- tipos_ingredientes
- ingredientes
- ingredientes_cervezas
*/

-- Tablas temporales
-- TMP_CERVECERIAS
create table tmp_cervecerias
(nombre text, ubicacion text, sitio_web text, instagram text);

insert into cervecerias(nombre, ubicacion_id, sitio_web, instagram)
select tmp.nombre,
       u.id ubicacion_id,
       tmp.sitio_web,
       tmp.instagram
from tmp_cervecerias tmp
 join ubicaciones u on (u.municipio || ', ' ||u.departamento) = tmp.ubicacion;

 -- TMP_CERVEZAS
 create table tmp_cervezas
(nombre text, cerveceria text, estilo text, ibu real, abv real);

insert into cervezas (nombre, estilo_id, cerveceria_id, ibu, abv)
select
    tmp.nombre,
    e.id estilo_id,
    cr.id cerveceria_id,
    ibu,
    abv
from tmp_cervezas tmp
    join cervecerias cr on tmp.cerveceria = cr.nombre
    join estilos e on tmp.estilo = e.nombre;

-- TMP_ENVASADOS_CERVEZAS
create table tmp_envasados_cervezas
(cerveceria text, cerveza text, envasado text, unidad_volumen text, volumen real);

insert into envasados_cervezas (cerveza_id, envasados_id, unidad_volumen_id, volumen)
select
    c.id cerveza_id,
    e.id envasado_id,
    uv.id unidad_volumen_id,
    tmp.volumen
from tmp_envasados_cervezas tmp
 join cervecerias cr on cr.nombre = tmp.cerveceria
 join cervezas c on cr.id = c.cerveceria_id
        and c.nombre = tmp.cerveza
join envasados e on tmp.envasado = e.nombre
join unidades_volumen uv on tmp.unidad_volumen = uv.nombre;

-- TMP_INGREDIENTES
create table tmp_ingredientes (ingrediente text, tipo_ingrediente text);

insert into ingredientes (nombre, tipo_ingrediente_id)
select
    tmp.ingrediente,
    ti.id  tipo_ingrediente_id
from tmp_ingredientes tmp
    join tipos_ingredientes ti on tmp.tipo_ingrediente = ti.nombre;


-- TMP_INGREDIENTES_CERVEZAS
create table tmp_ingredientes_cervezas
(cerveceria text, cerveza text, tipo_ingrediente text, ingrediente text );

insert into ingredientes_cervezas (cerveza_id, ingrediente_id) 
select
    c.id cerveza_id,
    i.id ingrediente_id
from tmp_ingredientes_cervezas tmp
join cervecerias cr on tmp.cerveceria = cr.nombre
join cervezas c on cr.id = c.cerveceria_id
            and tmp.cerveza = c.nombre
join tipos_ingredientes ti on ti.nombre = tmp.tipo_ingrediente
join ingredientes i on ti.id = i.tipo_ingrediente_id
    and tmp.ingrediente = i.nombre;