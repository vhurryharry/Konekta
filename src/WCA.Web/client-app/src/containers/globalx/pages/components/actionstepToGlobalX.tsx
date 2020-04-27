import * as React from 'react'

import "./actionstepToGlobalX.css"

export default class ActionstepToGlobalX extends React.Component {
    render(): JSX.Element {
        return (
            <>
                <div className="ms-Grid-row">
                    <div className="ms-Grid-col ms-sm12 center-align">
                        <h1>Passing your matter details to GlobalX...</h1>
                    </div>
                </div>
                <div className="ms-Grid-row">
                    <img src="/images/arc-arrow.svg" alt="" className="ms-Grid-col ms-sm12" />
                </div>
                <div className="ms-Grid-row center-align">
                    <div className="ms-Grid-col ms-sm2" />
                    <img src="/images/ActionStep logo_400x77.png" alt="Actionstep" className="ms-Grid-col ms-sm3" />
                    <img src="/images/Konekta_loading.svg" alt="Loading..." className="ms-Grid-col ms-sm2" height="50" />
                    <img src="/images/globalx-logo.png" alt="GlobalX" className="ms-Grid-col ms-sm3" />
                    <div className="ms-Grid-col ms-sm2" />
                </div>
            </>
        )
    }
}