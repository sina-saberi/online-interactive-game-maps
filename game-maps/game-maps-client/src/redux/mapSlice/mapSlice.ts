import { PayloadAction, createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { IMap } from "../../models/map";
import axios from "../../services/axios";
import { IMarker } from "../../models/marker";

const initialState: InitialState<IMap[]> = {}


export const GetMaps = createAsyncThunk("GetMaps", async (slug: string) => {
    let res = await axios.get<IMap[]>("/Map/GetMaps", { params: { slug } });
    return res.data;
})



const map = createSlice({
    name: "map",
    initialState,

    reducers: {
        setMapDetailt: (state, action: PayloadAction<IMap>) => {
            state.detail = action.payload;
        }
    },

    extraReducers: (b) => {
        b.addCase(GetMaps.fulfilled, (state, action) => {
            state.data = action.payload;
            state.loading = false;
        });
    }
})


export const { setMapDetailt } = map.actions;
export default map.reducer;