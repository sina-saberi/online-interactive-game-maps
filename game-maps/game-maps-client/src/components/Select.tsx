import React from 'react'

type OptionType = { value: string, id: string }
interface ISelectProps {
    data: OptionType[];
    placeholder?: string;
    label?: string;
    name: string;
    onChange: (id: OptionType["id"]) => void;
    value: OptionType["id"]
}
const Select = ({ data, placeholder, label, name, onChange, value }: ISelectProps) => {
    return (
        <label className='flex flex-col w-min' htmlFor={name}>
            {label}
            <select onChange={(e) => onChange(e.target.value)} id={name} value={value} name={name} className='outline-none'>
                {placeholder && <option value={""}>{placeholder}</option>}
                {data.map((item, index) =>
                    <option key={index} value={item.id}>{item.value}</option>
                )}
            </select>
        </label >
    )
}

export default Select