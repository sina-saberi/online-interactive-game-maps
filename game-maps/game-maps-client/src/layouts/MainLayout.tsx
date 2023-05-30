import React from 'react';
import { Cookies } from 'react-cookie';
import axios from '../services/axios';
import { createSearchParams, useNavigate } from 'react-router-dom';

interface IMainLayout {
    children: React.ReactElement
}
const MainLayout: React.FC<IMainLayout> = ({ children }) => {
    const navigate = useNavigate();

    axios.interceptors.request.use(x => {
        const cookies = new Cookies("GameMaps");
        const token = cookies.get("GameMaps");
        if (token)
            x.headers.Authorization = "Bearer " + token
        return x;
    }, e => e)

    axios.interceptors.response.use(x => x, e => {
        if (e.response.status && e.response.status === 401) {
            const cookies = new Cookies("GameMaps");
            if (cookies.get("GameMaps")) {
                cookies.remove("GameMaps")
            }
            navigate({
                pathname: "/auth/login",
                search: `${createSearchParams({ url: window.location.href })}`
            });
        }
        throw e
    })

    return (
        <React.Fragment>
            {children}
        </React.Fragment>
    )
}

export default MainLayout