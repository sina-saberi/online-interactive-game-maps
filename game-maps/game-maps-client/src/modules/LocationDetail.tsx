import React from 'react'
import { useAppDispatch, useAppSelector } from '../hooks/useRedux'
import { GetLocation, SetMarker } from '../redux/locationSlice/locationSlice';
import { IMarker } from '../models/marker';
import { unwrapResult } from '@reduxjs/toolkit';

const LocationDetail: React.FC<{ id: number, onClose?: () => void }> = ({ id, onClose }) => {
    const [data, setData] = React.useState<IMarker>({ isDone: false, locationId: id });

    const state = useAppSelector(x => x.location.detail);
    const dispatch = useAppDispatch();

    const getData = React.useCallback(async () => {
        try {
            let res = unwrapResult(await dispatch(GetLocation(id)));
            if (res.marker)
                setData({ ...res.marker });
        } catch (er) { }
    }, [dispatch, id]);

    React.useEffect(() => {
        getData();
    }, [getData]);

    const onClick = async () => {
        try {
            var res = unwrapResult(await dispatch(SetMarker(data)));
            setData(res);
            onClose && onClose()
        } catch (er) {

        }
    }

    return (
        <div>
            {(state && state.data) && (
                <div>
                    <h2 className='font-bold'>{state.data.title}</h2>
                    <div>
                        {(state.data.medias) && (
                            <div>
                                {state.data.medias.map((media, index) => (
                                    <div key={index}>
                                        <img src={`https://media.mapgenie.io/storage/media/${media.fileName}`} />
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>
                    <p>
                        {state.data.description}
                    </p>
                    <div>
                        <textarea value={data?.note || ""} onChange={(e) => { setData(p => ({ ...p, note: e.target.value })) }} rows={3} className='w-full outline-none border rounded-md p-2 resize-none'>sd</textarea>
                    </div>
                    <div>
                        <label className='select-none font-bold flex gap-3 my-3 justify-center items-center'>
                            mark as faund
                            <button onClick={(p) => { p.preventDefault(); setData(p => ({ ...p!, isDone: !p?.isDone })) }} className='w-6 h-6 p-0.5 rounded border flex justify-center items-center'>
                                {data?.isDone && <svg className='w-full h-full' xmlns="http://www.w3.org/2000/svg" zoomAndPan="magnify" viewBox="0 0 30 30.000001" preserveAspectRatio="xMidYMid meet" version="1.0"><defs><clipPath id="id1"><path d="M 2.328125 4.222656 L 27.734375 4.222656 L 27.734375 24.542969 L 2.328125 24.542969 Z M 2.328125 4.222656 " clipRule="nonzero" /></clipPath></defs><g clipPath="url(#id1)"><path fill="rgb(13.729858%, 12.159729%, 12.548828%)" d="M 27.5 7.53125 L 24.464844 4.542969 C 24.15625 4.238281 23.65625 4.238281 23.347656 4.542969 L 11.035156 16.667969 L 6.824219 12.523438 C 6.527344 12.230469 6 12.230469 5.703125 12.523438 L 2.640625 15.539062 C 2.332031 15.84375 2.332031 16.335938 2.640625 16.640625 L 10.445312 24.324219 C 10.59375 24.472656 10.796875 24.554688 11.007812 24.554688 C 11.214844 24.554688 11.417969 24.472656 11.566406 24.324219 L 27.5 8.632812 C 27.648438 8.488281 27.734375 8.289062 27.734375 8.082031 C 27.734375 7.875 27.648438 7.679688 27.5 7.53125 Z M 27.5 7.53125 " fillOpacity="1" fillRule="nonzero" /></g></svg>}
                            </button>
                        </label>
                    </div>
                    <div>
                        <button onClick={onClick} className='bg-emerald-600 text-white font-bold w-full rounded-md p-2'>submit</button>
                    </div>
                </div>
            )}
        </div>
    )
}

export default LocationDetail