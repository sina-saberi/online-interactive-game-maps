import { IMarker } from "./marker";

export interface ILocation {
    id: number;
    categorieName?: string,
    latitude: number,
    longitude: number,
    icon: string,
    marker?: IMarker;
    isDone: boolean
}