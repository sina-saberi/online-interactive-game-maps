import React from 'react'
import { useAppSelector } from '../hooks/useRedux';
import { Marker, Popup, useMapEvents, } from 'react-leaflet';
import Leaflet from 'leaflet';
import markers from "./marker.json";
import LocationDetail from './LocationDetail';


const Locations: React.FC<{ onZoomChange?: (zoom: number) => void, filter: { [name: string]: boolean } }> = ({ onZoomChange, filter }) => {
    const locations = useAppSelector(x => x.location);
    const [detail, setDetail] = React.useState<number | undefined>(undefined);
    const ref = React.useRef<Leaflet.Popup>(null);

    useMapEvents({
        zoomend: (props) => { onZoomChange && onZoomChange(props.sourceTarget._animateToZoom); },
    })

    return (
        <React.Fragment>
            {locations.data && locations.data?.filter(x => x.categorieName ? filter[x.categorieName] : true).map((loc, index) =>
                <Marker
                    opacity={loc.isDone ? 0.4 : 1.0}
                    icon={Leaflet.divIcon({
                        html: `<img style="position: absolute;top: -59px;left: -27.5px;scale: 0.5;width: ${markers[loc.icon as keyof typeof markers].width}px;height: ${markers[loc.icon as keyof typeof markers].height}px;object-fit: cover;object-position: 0 -${markers[loc.icon as keyof typeof markers].y}px;" src="${require("./markers.png")}"/>`
                    })}
                    eventHandlers={{
                        click: () => setDetail(loc.id),
                        popupclose: () => setDetail(undefined)
                    }}
                    autoPan
                    key={index} position={[loc.latitude, loc.longitude]}>
                    <Popup
                        ref={ref}

                    >
                        {detail && <LocationDetail onClose={() => {
                            ref.current?.close();
                        }} id={detail} />}
                    </Popup>
                </Marker>
            )}
        </React.Fragment>
    )
}

export default Locations