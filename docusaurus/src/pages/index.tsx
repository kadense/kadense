import type {ReactNode} from 'react';
import clsx from 'clsx';
import Link from '@docusaurus/Link';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import Layout from '@theme/Layout';
import HomepageFeatures from '@site/src/components/HomepageFeatures';
import Heading from '@theme/Heading';

import styles from './index.module.css';

function HomepageHeader() {
  const {siteConfig} = useDocusaurusContext();
  return (
      <header className={clsx('hero hero--primary', styles.heroBanner)}>
        <div className="row">
          <div className={clsx('col col--6')}>
            <div className="container">
              <Heading as="h1" className="hero__title">
                {siteConfig.title}
              </Heading>
              <p className={clsx('hero__subtitle', styles.hero__subtitle)}>{siteConfig.tagline}</p>
            </div>
          </div>
          <div className={clsx('col col--1')}>
            &nbsp;
          </div>
          <div className={clsx('col col-5 kadense_icon')}>
            &nbsp;
          </div>
        </div>
      </header>
  );
}

export default function Home(): ReactNode {
  const {siteConfig} = useDocusaurusContext();
  return (
    <Layout
      title={siteConfig.title}
      description="Kadense is a framework for building applications and services on Kubernetes.">
      <HomepageHeader />
      <main>
        <HomepageFeatures />
      </main>
    </Layout>
  );
}
