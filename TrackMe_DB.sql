PGDMP     +    7            	    {           TrackMe    15.2    15.2     �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    16611    TrackMe    DATABASE     �   CREATE DATABASE "TrackMe" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_United States.1252';
    DROP DATABASE "TrackMe";
                postgres    false            �            1259    16618    application    TABLE     �   CREATE TABLE public.application (
    app_id integer,
    app_name character varying(255),
    app_category character varying(50)
);
    DROP TABLE public.application;
       public         heap    postgres    false            �            1259    16615    sessions    TABLE     �   CREATE TABLE public.sessions (
    app_id integer,
    start_time timestamp without time zone,
    end_time timestamp without time zone
);
    DROP TABLE public.sessions;
       public         heap    postgres    false            �          0    16618    application 
   TABLE DATA           E   COPY public.application (app_id, app_name, app_category) FROM stdin;
    public          postgres    false    215   A       �          0    16615    sessions 
   TABLE DATA           @   COPY public.sessions (app_id, start_time, end_time) FROM stdin;
    public          postgres    false    214          �   �   x�U�=
1��zs�=�d����^��B,]���a�iț�v+���y<_������5�9�s�C�S��̩xX�T=lsj�9uǜ���<k�����pF 	g�p��g
�pƠ	g�pa�M�Ȣ��"����"K�YdY8��g�U�,�&�Eօ�Ȇ|����B�ɬ��      �   �   x���K1C��\`P��7=�?#�J������N@��M��3�������9��jZ@�:����4��e)389�_>�oK�M-r�O"��i%ʶ�d��t�,'M�	٣��C���JU����ڒ+*Y�:�E�n�̪�X�T-�u�շ��b��c5���ڎՁ5��[z���M]7[��B��- 3���6��}���f�.@au�ӒV0���h���]�uh ��[z2.%Nr�/����(�     