import * as React from 'react';
import { SendFirstTitlePolicyRequestResponse } from 'utils/wcaApiTypes';

interface IProps {
    sendFirstTitlePolicyRequestResponse: SendFirstTitlePolicyRequestResponse
}

interface IState { }

class PolicySuccess extends React.Component<IProps, IState> {
    constructor(props: IProps) {
        super(props);

        this.state = {}
    }

    render() {
        const { sendFirstTitlePolicyRequestResponse } = this.props;

        return (
            <>
                <h2>
                    Policy Order ID
                </h2>

                <div>
                    {sendFirstTitlePolicyRequestResponse.policyNumber}
                </div>

                <h2>
                    Price
                </h2>

                <div>
                    Premium: $ {sendFirstTitlePolicyRequestResponse.price && sendFirstTitlePolicyRequestResponse.price.premium} <br />
                    GSTOnPremium: $ {sendFirstTitlePolicyRequestResponse.price && sendFirstTitlePolicyRequestResponse.price.gstOnPremium} <br />
                    StampDuty: $ {sendFirstTitlePolicyRequestResponse.price && sendFirstTitlePolicyRequestResponse.price.stampDuty}
                </div>

                <h3>
                    Disbursements for these prices have been created on Actionstep.
                </h3>

                {sendFirstTitlePolicyRequestResponse.attachmentPaths && sendFirstTitlePolicyRequestResponse.attachmentPaths.length > 0 &&
                    <>
                        <h3>
                            Policy PDF has been saved to Actionstep.
                        </h3>

                        {sendFirstTitlePolicyRequestResponse.attachmentPaths.map(attachment => {
                            return (
                                <>
                                    <a href={attachment.fileUrl} rel="noopener noreferrer" target="_blank" className="ft-attachment-link">
                                        - {attachment.fileName}
                                    </a>
                                    <br />
                                </>
                            );
                        })}
                    </>
                }
            </>
        )
    }
}

export default PolicySuccess;