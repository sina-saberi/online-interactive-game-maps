import React, { useRef } from 'react'
import { useNavigate, useParams } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../../hooks/useRedux';
import { GetMaps, setMapDetailt } from '../../redux/mapSlice/mapSlice';
import Select from '../../components/Select';
import { GetGroupsAndCategories } from '../../redux/groupSlice/groupSlice';
import { unwrapResult } from '@reduxjs/toolkit';
import Leaflet from 'leaflet';
import "leaflet/dist/leaflet.css";
import { GetLocations } from '../../redux/locationSlice/locationSlice';
import { MapContainer, TileLayer } from 'react-leaflet';
import Locations from '../../modules/Locations';
import { useCookies } from 'react-cookie';

const Map = () => {
    const { slug } = useParams();
    const navigate = useNavigate();
    const mapRef = useRef<Leaflet.Map>(null)
    const dispatch = useAppDispatch();
    const maps = useAppSelector(x => x.map);
    const groups = useAppSelector(x => x.groups);
    const [menu, setMenu] = React.useState(false);
    const [map, setMap] = React.useState("");
    const [filters, setFilters] = React.useState<{ [name: string]: boolean }>({});
    const [coockie] = useCookies(["GameMaps"]);

    const getLocations = React.useCallback((map: string) => {
        if (map)
            dispatch(GetLocations(map));
    }, [slug])

    const manageMap = React.useCallback(async (id: string) => {
        setMap(id)
        try {
            let mapDetail = maps.data?.find(x => x.slug === id)
            if (mapDetail) {
                dispatch(setMapDetailt(mapDetail))
                if (slug) {
                    let res = unwrapResult(await dispatch(GetGroupsAndCategories(slug)));
                    setFilters(p => {
                        let obj: { [name: string]: boolean } = {};
                        res.forEach((groups) => {
                            groups.categories.forEach((categorie) => {
                                obj[categorie.title] = true;
                            })
                        })
                        return obj;
                    });
                    getLocations(id);
                }
            }
        } catch (er) { }
    }, [map, dispatch, slug, maps, getLocations]);

    const GetData = React.useCallback(async () => {
        try {
            if (slug) {
                unwrapResult(await dispatch(GetMaps(slug)));
            }
        } catch (er) {

        }
    }, [slug, dispatch])

    React.useEffect(() => {
        GetData();
    }, [GetData]);




    return (
        <div className='h-screen'>
            <div style={{ zIndex: 300000 }} className={`z-50 max-w-[300px] min-w-[300px] h-full bg-white fixed top-0 left-0 transition-all ${menu ? "-translate-x-full" : ""}`}>
                <div className='w-full h-full relative'>
                    <button onClick={() => setMenu(!menu)} className='absolute bg-white px-2 py rotate-90 top-40 -right-10'>close</button>
                    <div className='w-full h-full p-4 overflow-y-auto'>
                        <div className='flex justify-between'>
                            {maps.data &&
                                <Select
                                    name=''
                                    onChange={(id) => manageMap(id)}
                                    value={map}
                                    placeholder={!map ? 'select' : undefined}
                                    data={maps.data?.map((item) => {
                                        return { id: item.slug, value: item.title }
                                    })} />
                            }
                            {!coockie.GameMaps && <button className='bg-blue-400 text-white px-3 py rounded-md' onClick={() => navigate({ pathname: "/auth/login" })}>{"login"}</button>}
                        </div>
                        {groups.data &&
                            <React.Fragment>
                                {groups.loading && <div>lodaing</div>}
                                <ul className='mt-3'>
                                    {groups.data.map((group, index) =>
                                        <li key={index}>
                                            <h3 className='font-bold text-lg'>
                                                <button
                                                    onClick={() => {
                                                        setFilters(p => {
                                                            let obj: { [name: string]: boolean } = {};
                                                            group.categories.forEach((categorie) => {
                                                                obj[categorie.title] = !p[categorie.title];
                                                            })
                                                            return { ...p, ...obj };
                                                        })
                                                    }}
                                                >{group.title}</button>
                                            </h3>
                                            <ul className='flex flex-wrap'>
                                                {group.categories.map((item, key) =>
                                                    <li className={`px-1 py-0.5 m-1`} key={key}>
                                                        <button
                                                            onClick={() => {
                                                                setFilters(p => ({ ...p, [item.title]: !p[item.title] }))
                                                            }}
                                                            className={`${filters[item.title] ? "" : "line-through opacity-60"}`}>
                                                            {item.title}
                                                        </button>
                                                    </li>
                                                )}
                                            </ul>
                                        </li>
                                    )}
                                </ul>
                            </React.Fragment>
                        }
                    </div>
                </div>
            </div >
            {maps.detail != undefined && (
                <React.Fragment>
                    <MapContainer
                        ref={mapRef}
                        style={{ width: "100%", height: "100%" }}
                        center={[maps.detail.initialLat, maps.detail.initialLon]}
                        zoom={maps.detail.initialZoom}
                        minZoom={maps.detail.minZoom}
                        maxZoom={maps.detail.maxZoom}
                        zoomControl={false}
                        maxBoundsViscosity={1.0}
                        boundsOptions={{
                            animate: false,
                            duration: 0,
                            padding: [0, 0],
                            easeLinearity: 0,
                            noMoveStart: false,
                        }}
                        scrollWheelZoom={true}
                    >
                        <TileLayer
                            bounds={[
                                [maps.detail.endtLatBound, maps.detail.endtLonBound],
                                [maps.detail.startLatBound, maps.detail.startLonBound]
                            ]}
                            url={`https://tiles.mapgenie.io${maps.detail.path}/{z}/{x}/{y}.${maps.detail.extension}`} />
                        <Locations filter={filters} />
                    </MapContainer>
                </React.Fragment>
            )}
        </div>
    )
}

export default Map;