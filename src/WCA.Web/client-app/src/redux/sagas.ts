
import { all, fork } from 'redux-saga/effects';

import commonSaga from 'redux/common/saga';
import pexaSaga from 'containers/pexa/redux/saga';
import globalXSaga from 'containers/globalx/redux/saga';
import settlementInfoSaga from 'containers/calculators/settlement/redux/saga';
import firstTitleSaga from 'containers/firsttitle/redux/saga';

export default function* rootSaga() {
    yield all([
        fork(commonSaga),
        fork(pexaSaga),
        fork(globalXSaga),
        fork(settlementInfoSaga),
        fork(firstTitleSaga),
    ])
}