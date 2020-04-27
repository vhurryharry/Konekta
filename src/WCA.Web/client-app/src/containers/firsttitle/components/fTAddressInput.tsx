import * as React from 'react'

import { TextField } from 'office-ui-fabric-react/lib/TextField';
import { Dropdown } from 'office-ui-fabric-react/lib/Dropdown';

import { FTParty } from 'utils/wcaApiTypes';
import { StreetTypes } from './constants'

interface IProps {
    party: FTParty;
    prefix: string;
    isValid: (value: any) => boolean;
    setMatterInformation: (keyPath: string, newValue: any) => void;
}

export default class FTAddressInput extends React.Component<IProps> {
    render(): JSX.Element {
        const { party, setMatterInformation, isValid, prefix } = this.props;

        return (
            <>
                <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg3"
                    label="Country"
                    id={prefix + "_country"}
                    value={party.country || ""}
                    errorMessage={isValid(party.country) ? "" : " "}
                    onChange={(ev, newValue) => setMatterInformation("country", newValue)}
                />

                <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg3"
                    label="State"
                    id={prefix + "_state"}
                    value={party.stateProvince || ""}
                    errorMessage={isValid(party.stateProvince) ? "" : " "}
                    onChange={(ev, newValue) => setMatterInformation("stateProvince", newValue)}
                />

                <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg3"
                    label="City"
                    id={prefix + "_city"}
                    value={party.city || ""}
                    errorMessage={isValid(party.city) ? "" : " "}
                    onChange={(ev, newValue) => setMatterInformation("city", newValue)}
                />

                <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg3"
                    label="PostCode"
                    id={prefix + "_postcode"}
                    value={party.postCode || ""}
                    errorMessage={isValid(party.postCode) ? "" : " "}
                    onChange={(ev, newValue) => setMatterInformation("postCode", newValue)}
                />

                <div className="ms-Grid-col ms-sm12">
                    <h4>Street Address</h4>
                </div>

                <div className="ms-Grid-col ms-sm11 ms-smPush1 ms-Grid-row">
                    <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg3"
                        label="Building Name"
                        id={prefix + "_building_name"}
                        value={party.buildingName || ""}
                        onChange={(ev, newValue) => setMatterInformation("buildingName", newValue)}
                    />

                    <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg3"
                        label="Floor No"
                        id={prefix + "_floor_no"}
                        value={party.floorNo || ""}
                        onChange={(ev, newValue) => setMatterInformation("floorNo", newValue)}
                    />

                    <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg3"
                        label="Unit No"
                        id={prefix + "_unit_no"}
                        value={party.unitNo || ""}
                        onChange={(ev, newValue) => setMatterInformation("unitNo", newValue)}
                    />

                    <TextField className="ms-Grid-col ms-sm12 ms-md6 ms-lg3"
                        label="Street No"
                        id={prefix + "_street_no"}
                        value={party.streetNo || ""}
                        onChange={(ev, newValue) => setMatterInformation("streetNo", newValue)}
                    />

                    <Dropdown className="ms-Grid-col ms-sm12 ms-md6"
                        label="Street Type"
                        id={prefix + "_street_type"}
                        options={StreetTypes}
                        placeholder="Please select..."
                        selectedKey={party.streetType}
                        errorMessage={isValid(party.streetType) ? "" : " "}
                        onChange={(event, item) => setMatterInformation("streetType", item!.key.toString())}
                    />

                    <TextField className="ms-Grid-col ms-sm12 ms-md6"
                        label="Street Name"
                        id={prefix + "_street_name"}
                        value={party.streetName || ""}
                        onChange={(ev, newValue) => setMatterInformation("streetName", newValue)}
                    />
                </div>
            </>
        );
    }
}