import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { ILocation } from "../../models/location";
import axios from "../../services/axios";
import { ILocationDetail } from "../../models/locationDetail";
import { IMarker } from "../../models/marker";

const initialState: InitialState<ILocation[], { data?: ILocationDetail, loading: boolean }> = {};



export const GetLocations = createAsyncThunk("getLocations", async (slug: string) => {
    let res = await axios.get<ILocation[]>("/Map/GetLocationsByMapSlug", { params: { slug } });
    return res.data;
})

export const GetLocation = createAsyncThunk("GetLocation", async (id: number) => {
    let res = await axios.get<ILocationDetail>("/Map/GetLocation", { params: { id } });
    return res.data;
})

export const SetMarker = createAsyncThunk("SetMarker", async (marker: IMarker) => {
    let res = await axios.post<IMarker>("/Map/MarkLocationByUser", marker);
    return res.data;
});

const location = createSlice({
    name: "location",
    initialState,
    reducers: {

    },
    extraReducers(builder) {
        builder.addCase(GetLocations.fulfilled, (state, action) => {
            state.data = action.payload;
        });
        builder.addCase(GetLocation.pending, (state) => {
            state.detail = {
                loading: false,
            }
        })
        builder.addCase(GetLocation.fulfilled, (state, action) => {
            state.detail = {
                ...state.detail,
                loading: false,
                data: action.payload
            }
        });
        builder.addCase(SetMarker.fulfilled, (state, action) => {
            if (state.data) {
                state.data = state.data.map(x => {
                    if (x.id === action.payload.locationId) {
                        const ong = { ...x, isDone: action.payload.isDone } as ILocation
                        return ong
                    }
                    return x
                })
            }
        })
    },
})


export default location.reducer